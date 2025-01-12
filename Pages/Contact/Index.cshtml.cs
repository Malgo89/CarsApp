using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarsApp.Pages.Contact;

public class IndexModel : PageModel
{
    private readonly ILogger<PrivacyModel> _logger;

    public IndexModel(ILogger<PrivacyModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
    }
}

