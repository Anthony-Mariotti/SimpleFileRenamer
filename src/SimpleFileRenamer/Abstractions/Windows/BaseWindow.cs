using NPOI.POIFS.Properties;

namespace SimpleFileRenamer.Abstractions.Windows;

public abstract class BaseWindow : Form
{
    private const string ShowDialog_Message = "Use ShowDialog() from the created window not the built in methods";

    [Obsolete(ShowDialog_Message)]
    public new DialogResult ShowDialog()
    {
        if (!IsDisposed)
        {
            return base.ShowDialog();
        }

        return DialogResult.None;
    }

    [Obsolete(ShowDialog_Message)]
    public new DialogResult ShowDialog(IWin32Window parent)
    {
        if (!IsDisposed)
        {
            return base.ShowDialog(parent);
        }

        return DialogResult.None;
    }
}
