using System;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;

namespace DevilDesireDevLib.Implementation.Extensions
{
    public static class MessageDialogExtensions
    {
        private static TaskCompletionSource<MessageDialog> m_CurrentDialogShowRequest;

        public static async Task<IUICommand> ShowAsyncQueue(this MessageDialog dialog)
        {
            if (!Window.Current.Dispatcher.HasThreadAccess)
            {
                throw new InvalidOperationException("This method can only be invoked from UI thread.");
            }

            while (m_CurrentDialogShowRequest != null)
            {
                await m_CurrentDialogShowRequest.Task;
            }

            var request = m_CurrentDialogShowRequest = new TaskCompletionSource<MessageDialog>();
            var result = await dialog.ShowAsync();
            m_CurrentDialogShowRequest = null;
            request.SetResult(dialog);

            return result;
        }
    }
}