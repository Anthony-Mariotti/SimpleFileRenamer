using System.Diagnostics;
using System.Runtime.InteropServices;

namespace SimpleFileRenamer.Controls;
public partial class SyncedListView : ListView
{
    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern nint SendMessage(nint hWnd, uint Msg, nint wParam, nint lParam);

    private const uint WM_VSCROLL = 0x0115;
    private const int SB_THUMBTRACK = 5;
    private const int SB_LINEUP = 0;
    private const int SB_LINEDOWN = 1;
    private const int SB_PAGEUP = 2;
    private const int SB_PAGEDOWN = 3;

    public SyncedListView Partner { get; set; } = default!;

    // Flag to avoid recursive message sending
    private bool isSyncing = false;

    private bool debug = false;

    protected override void WndProc(ref Message m)
    {
        base.WndProc(ref m);

        if (Partner != null && m.Msg == WM_VSCROLL && !isSyncing)
        {
            isSyncing = true;

            int scrollEventType = (int)m.WParam.ToInt64() & 0xFFFF;

            Debug.WriteLineIf(debug && scrollEventType == SB_THUMBTRACK,
                   $"WM_VSCROLL/SB_THUMBTRACK - {Partner.Handle}, {m.Msg}, {m.WParam}, {m.LParam}", "[ScrollSync]");

            Debug.WriteLineIf(debug && scrollEventType == SB_LINEUP,
                   $"WM_VSCROLL/SB_LINEUP - {Partner.Handle}, {m.Msg}, {m.WParam}, {m.LParam}", "[ScrollSync]");

            Debug.WriteLineIf(debug && scrollEventType == SB_LINEDOWN,
                   $"WM_VSCROLL/SB_LINEDOWN - {Partner.Handle}, {m.Msg}, {m.WParam}, {m.LParam}", "[ScrollSync]");

            Debug.WriteLineIf(debug && scrollEventType == SB_PAGEUP,
                   $"WM_VSCROLL/SB_PAGEUP - {Partner.Handle}, {m.Msg}, {m.WParam}, {m.LParam}", "[ScrollSync]");

            Debug.WriteLineIf(debug && scrollEventType == SB_PAGEDOWN,
                   $"WM_VSCROLL/SB_PAGEDOWN - {Partner.Handle}, {m.Msg}, {m.WParam}, {m.LParam}", "[ScrollSync]");

            Debug.WriteLineIf(
                debug &&
                (scrollEventType != SB_PAGEDOWN || scrollEventType != SB_PAGEUP ||
                scrollEventType != SB_LINEDOWN || scrollEventType != SB_LINEUP ||
                scrollEventType != SB_THUMBTRACK),
                   $"WM_VSCROLL/UNKNOWN - {Partner.Handle}, {m.Msg}, {m.WParam}, {m.LParam}", "[ScrollSync]");

            if (scrollEventType == SB_THUMBTRACK)
            {
                int position = unchecked((short)((int)m.WParam >> 16));

                if (Partner.Items.Count > position)
                {
                    Partner.TopItem = Partner.Items[position];
                }
            }
            else if (scrollEventType == SB_LINEUP || scrollEventType == SB_LINEDOWN ||
                     scrollEventType == SB_PAGEUP || scrollEventType == SB_PAGEDOWN)
            {
                int currentIndex = TopItem?.Index ?? 0;

                if (currentIndex >= 0 && currentIndex < Partner.Items.Count)
                {
                    Partner.TopItem = Partner.Items[currentIndex];
                }
            }

            isSyncing = false;
        }
    }
}
