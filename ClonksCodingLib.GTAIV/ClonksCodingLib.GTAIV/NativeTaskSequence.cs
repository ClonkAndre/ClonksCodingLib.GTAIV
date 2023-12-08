using System;

using CCL.GTAIV.TaskController;

using IVSDKDotNet;
using static IVSDKDotNet.Native.Natives;

namespace CCL.GTAIV
{
    public class NativeTaskSequence : IDisposable
    {

        #region Variables and Properties
        // Variables
        private int handle;
        private bool closed;

        private PedTaskController tempTaskController;

        // Properties
        public int Handle
        {
            get { return handle; }
            private set { handle = value; }
        }
        public bool Closed
        {
            get { return closed; }
            private set { closed = value; }
        }
        public PedTaskController AddTask
        {
            get
            {
                if (Closed)
                    return null;

                return tempTaskController;
            }
        }
        #endregion

        #region Constructor
        public NativeTaskSequence()
        {
            OPEN_SEQUENCE_TASK(out int taskSeq);
            Handle = taskSeq;
            tempTaskController = new PedTaskController(0);
        }
        #endregion

        #region Methods
        public void Dispose()
        {
            CLEAR_SEQUENCE_TASK(Handle);
        }
        
        public void Perform(IVPed target)
        {
            if (target == null)
                return;
            if (!target.Exists())
                return;

            if (!Closed)
            {
                CLOSE_SEQUENCE_TASK(Handle);
                Closed = true;
            }

            int pedHandle = target.GetHandle();
            CLEAR_CHAR_TASKS(pedHandle);
            target.BlockPermanentEvents(true);
            _TASK_PERFORM_SEQUENCE(pedHandle, Handle);
        }
        #endregion

    }
}
