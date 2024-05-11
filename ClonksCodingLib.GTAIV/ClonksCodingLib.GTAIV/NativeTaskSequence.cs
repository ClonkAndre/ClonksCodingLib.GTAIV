using CCL.GTAIV.TaskController;

using IVSDKDotNet;
using static IVSDKDotNet.Native.Natives;

namespace CCL.GTAIV
{
    /// <summary>
    /// Allows you to perform a sequence on a <see cref="IVPed"/>.
    /// </summary>
    public class NativeTaskSequence
    {
        #region Variables and Properties
        // Variables
        private int handle;
        private bool closed;

        /// <summary>
        /// The handle of the <see cref="IVPed"/> this sequence was performed on.
        /// </summary>
        public int TargetPedHandle;

        // Properties
        /// <summary>
        /// The handle of the created sequence.
        /// </summary>
        public int Handle
        {
            get { return handle; }
            private set { handle = value; }
        }
        /// <summary>
        /// Gets if this sequence is closed.<br/>
        /// A closed sequence cannot be started again.
        /// </summary>
        public bool Closed
        {
            get { return closed; }
            private set { closed = value; }
        }
        /// <summary>
        /// Allows you to add tasks to this sequence.
        /// </summary>
        public PedTaskController AddTask
        {
            get
            {
                if (Closed)
                    return null;

                return PedTaskController.TempTaskController;
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance of the <see cref="NativeTaskSequence"/> class and opens a new sequence task.
        /// </summary>
        public NativeTaskSequence()
        {
            // Open new sequence task
            OPEN_SEQUENCE_TASK(out handle);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Starts performing the added tasks in this sequence of the <paramref name="target"/> <see cref="IVPed"/>.
        /// </summary>
        /// <param name="target">The <see cref="IVPed"/> to perform the sequence on.</param>
        /// <param name="clearTargetTasksAndBlockEventsBeforeRun">If the current tasks of the <paramref name="target"/> should be cleared, and if the <paramref name="target"/> should block permanent events before performing the sequence.</param>
        public void Perform(IVPed target, bool clearTargetTasksAndBlockEventsBeforeRun = true)
        {
            if (Closed)
                return;
            if (target == null)
                return;
            if (!target.Exists())
                return;

            // Close the sequence task
            if (!Closed)
            {
                CLOSE_SEQUENCE_TASK(Handle);
                Closed = true;
            }

            // Set stuff
            TargetPedHandle = target.GetHandle();

            if (clearTargetTasksAndBlockEventsBeforeRun)
            {
                CLEAR_CHAR_TASKS(TargetPedHandle);
                target.BlockPermanentEvents(true);
            }

            // Perform the sequence on the target ped
            _TASK_PERFORM_SEQUENCE(TargetPedHandle, Handle);
            CLEAR_SEQUENCE_TASK(Handle);
        }
        #endregion

        #region Functions
        /// <summary>
        /// Gets the progress of this sequence for the <see cref="IVPed"/> given in the <see cref="Perform(IVPed, bool)"/> method.
        /// </summary>
        /// <returns>
        /// <b>-1</b> if no sequence is currently active on the <see cref="IVPed"/> (or if the sequence completed).<br/>
        /// Otherwise return the number of the currently active task (Zero based).
        /// 
        /// <para>
        /// <b>- - - Example - - -</b><br/>
        /// You've added the following tasks to this sequence:<br/>
        /// - (<b>0</b>) <see cref="PedTaskController.UseMobilePhone()"/>,<br/>
        /// - (<b>1</b>) <see cref="PedTaskController.RunTo(System.Numerics.Vector3)"/>,<br/>
        /// - (<b>2</b>) <see cref="PedTaskController.HandsUp(uint)"/><br/>
        /// Now if the <see cref="IVPed"/> reached the <see cref="PedTaskController.HandsUp(uint)"/> task, this function would return <b>2</b>.
        /// </para>
        /// 
        /// </returns>
        public int GetProgress()
        {
            if (TargetPedHandle == 0)
                return -1;
            if (!DOES_CHAR_EXIST(TargetPedHandle))
                return -1;

            GET_SEQUENCE_PROGRESS(TargetPedHandle, out int progress);
            return progress;
        }
        #endregion
    }
}
