using lib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrescriptionService.DAP
{
    public interface IPrescriptionRepo
    {
        public IEnumerable<PrescriptionOut> GetPrescriptionsExpiringLatest(DateTime expiringDate);
        public IEnumerable<PrescriptionOut> GetPrescriptionsForUser(string username, string password);
        public bool MarkPrescriptionWarningSent(long prescriptionId);
        public IEnumerable<Patient> GetAllPatients();
        public IEnumerable<PrescriptionOut> GetAllPrescriptions();
        public IEnumerable<Pharmacy> GetAllPharmacies();
    }

}
