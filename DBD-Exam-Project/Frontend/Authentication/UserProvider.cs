using lib.DTO;

namespace Frontend.Authentication;

public class UserProvider
{
    private PersonDto? _currentUser;
    public PersonDto? CurrentUser
    {
        get => _currentUser;
        set
        {
            _currentUser = value;
            OnUserChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public event EventHandler<EventArgs> OnUserChanged;
}
