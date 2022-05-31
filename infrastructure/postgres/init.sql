CREATE DATABASE prescription_db;
\connect prescription_db

CREATE SCHEMA "prescriptions"
CREATE TABLE IF NOT EXISTS prescriptions.login_info(
   id SERIAL PRIMARY KEY,
   username varchar(64) NOT NULL UNIQUE,
   password varchar(64) NOT NULL,
   salt varchar(32) NOT NULL
);

CREATE TABLE IF NOT EXISTS prescriptions.address(
   id SERIAL PRIMARY KEY,
   streetname varchar(64) NOT NULL,
   streetnumber varchar(8) NOT NULL,
   zipcode varchar(32) NOT NULL,
   latitude double,
   longitude double
);

CREATE TABLE IF NOT EXISTS prescriptions.pharmacy(
   id SERIAL PRIMARY KEY,
   pharmacy_name varchar(64) NOT NULL,
   address_id int,
   CONSTRAINT fk_address
      FOREIGN KEY(address_id) 
	  REFERENCES prescriptions.address(id)
);

CREATE TABLE IF NOT EXISTS prescriptions.user_role(
   name varchar(32) NOT NULL,
   id SERIAL PRIMARY KEY
);

CREATE TABLE IF NOT EXISTS prescriptions.user_permission(
   id SERIAL PRIMARY KEY
);

CREATE TABLE IF NOT EXISTS prescriptions.personal_data(
   id SERIAL PRIMARY KEY,
   first_name varchar(64) NOT NULL,
   last_name varchar(64) NOT NULL,
   email VARCHAR(64),
   login_id int UNIQUE,
   role_id int,
   address_id int,
   CONSTRAINT fk_address
      FOREIGN KEY(address_id) 
	  REFERENCES prescriptions.address(id),
   CONSTRAINT fk_login
      FOREIGN KEY(login_id) 
	  REFERENCES prescriptions.login_info(id),
   CONSTRAINT fk_role
      FOREIGN KEY(role_id) 
	  REFERENCES prescriptions.user_role(id)
);

CREATE TABLE IF NOT EXISTS prescriptions.patient(
   id SERIAL PRIMARY KEY,
   cpr varchar(10) NOT NULL,
   personal_data_id int NOT NULL,
   CONSTRAINT fk_personal_data
      FOREIGN KEY(personal_data_id) 
	   REFERENCES prescriptions.personal_data(id)   
);

CREATE TABLE IF NOT EXISTS prescriptions.pharmaceut(
   id SERIAL PRIMARY KEY,
   pharmacy_id int,
   personal_data_id int NOT NULL,
   CONSTRAINT fk_personal_data
      FOREIGN KEY(personal_data_id) 
	   REFERENCES prescriptions.personal_data(id),
   CONSTRAINT fk_pharmacy
      FOREIGN KEY(pharmacy_id) 
	   REFERENCES prescriptions.pharmacy(id)
);

CREATE TABLE IF NOT EXISTS prescriptions.doctor(
   id SERIAL PRIMARY KEY,
   personal_data_id int NOT NULL,
   CONSTRAINT fk_personal_data
      FOREIGN KEY(personal_data_id) 
	   REFERENCES prescriptions.personal_data(id)
);

CREATE TABLE IF NOT EXISTS prescriptions.medicine(
   id SERIAL PRIMARY KEY,
   name varchar(64) NOT NULL
);

CREATE TABLE IF NOT EXISTS prescriptions.prescription(
   id BIGSERIAL PRIMARY KEY,
   expiration DATE,
   expiration_warning_sent boolean,
   creation TIMESTAMPTZ NOT NULL,
   medicine_id int NOT NULL,
   prescribed_by int NOT NULL,
   prescribed_to int NOT NULL,
   prescribed_to_cpr varchar(10) NOT NULL,
   last_administered_by int,
   CONSTRAINT fk_medicine
      FOREIGN KEY(medicine_id) 
	   REFERENCES prescriptions.medicine(id),
   CONSTRAINT fk_prescriber
      FOREIGN KEY(prescribed_by) 
	   REFERENCES prescriptions.doctor(id),
   CONSTRAINT fk_prescribee
      FOREIGN KEY(prescribed_to) 
	   REFERENCES prescriptions.patient(id),
   CONSTRAINT fk_administered_by
      FOREIGN KEY(last_administered_by) 
	   REFERENCES prescriptions.pharmaceut(id)
);



CREATE TABLE IF NOT EXISTS prescriptions.user_role_user_permission(
   role_id int,
   permission_id int,
   PRIMARY KEY(role_id, permission_id),
   CONSTRAINT fk_role
      FOREIGN KEY(role_id) 
	   REFERENCES prescriptions.user_role(id),
   CONSTRAINT fk_permission
      FOREIGN KEY(permission_id) 
	   REFERENCES prescriptions.user_permission(id)
);

ALTER TABLE prescriptions.prescription ENABLE ROW LEVEL SECURITY;

CREATE ROLE patient;
GRANT SELECT ON prescriptions.prescription TO patient;
GRANT SELECT ON prescriptions.medicine TO patient;

CREATE ROLE doctor;
GRANT SELECT, UPDATE, INSERT ON prescriptions.prescription TO doctor;
GRANT SELECT ON prescriptions.patient TO doctor;
GRANT SELECT ON prescriptions.personal_data TO doctor;
GRANT SELECT ON prescriptions.medicine TO doctor;

CREATE ROLE pharmaceut;
GRANT SELECT, UPDATE ON prescriptions.prescription TO pharmaceut;

CREATE POLICY prescription_patient ON prescriptions.prescription TO patient
    USING (prescribed_to_cpr = current_user);

CREATE USER renewalservice WITH ENCRYPTED PASSWORD 'renewal';
GRANT SELECT ON prescriptions.prescription TO patient;


create OR replace function prescriptions.create_patient(_username varchar, _password_hashed varchar, _password_salt varchar, _password_raw varchar)
returns int
language plpgsql STRICT VOLATILE SECURITY DEFINER
as
$$
begin
        INSERT INTO prescriptions.login_info(username,password,salt)
            VALUES (_username, _password_hashed, _password_salt);
           
        execute 'create user "'||_username::varchar||'" with password '||''''||_password_raw::varchar||'''';
       
       execute 'GRANT CONNECT ON DATABASE prescription_db TO "'||_username::varchar||'"';
      execute 'GRANT USAGE ON SCHEMA prescriptions TO "'||_username::varchar||'"';
     execute 'GRANT patient TO "'||_username::varchar||'"';
           
   
   return 1;
end;
$$;

create OR replace function prescriptions.create_doctor(_username varchar, _password_hashed varchar, _password_salt varchar, _password_raw varchar)
returns int
language plpgsql STRICT VOLATILE SECURITY DEFINER
as
$$
begin
        INSERT INTO prescriptions.login_info(username,password,salt)
            VALUES (_username, _password_hashed, _password_salt);
           
        execute 'create user "'||_username::varchar||'" with password '||''''||_password_raw::varchar||'''';
       
       execute 'GRANT CONNECT ON DATABASE prescription_db TO "'||_username::varchar||'"';
      execute 'GRANT USAGE ON SCHEMA prescriptions TO "'||_username::varchar||'"';
     execute 'GRANT doctor TO "'||_username::varchar||'"';
           
   
   return 1;
end;
$$;

create OR replace function prescriptions.create_pharmaceut(_username varchar, _password_hashed varchar, _password_salt varchar, _password_raw varchar)
returns int
language plpgsql STRICT VOLATILE SECURITY DEFINER
as
$$
begin
        INSERT INTO prescriptions.login_info(username,password,salt)
            VALUES (_username, _password_hashed, _password_salt);
           
        execute 'create user "'||_username::varchar||'" with password '||''''||_password_raw::varchar||'''';
       
       execute 'GRANT CONNECT ON DATABASE prescription_db TO "'||_username::varchar||'"';
      execute 'GRANT USAGE ON SCHEMA prescriptions TO "'||_username::varchar||'"';
     execute 'GRANT pharmaceut TO "'||_username::varchar||'"';
           
   
   return 1;
end;
$$;