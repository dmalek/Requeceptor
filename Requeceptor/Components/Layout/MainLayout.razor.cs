using MudBlazor;

namespace Requeceptor.Components.Layout;

public partial class MainLayout
{
    bool _drawerOpen = true;

    MudTheme MyCustomTheme = new MudTheme()
    {
        PaletteLight = new PaletteLight()
        {
            Primary = "#003049",
            Secondary = Colors.Green.Accent4,
            AppbarBackground = "#003049",
        },
        PaletteDark = new PaletteDark()
        {
            Primary = Colors.Blue.Lighten1
        },

        LayoutProperties = new LayoutProperties()
        {
            DrawerWidthLeft = "260px",
            DrawerWidthRight = "300px"
        }
    };

    void DrawerToggle()
    {
        _drawerOpen = !_drawerOpen;
        StateHasChanged();
    }
}
