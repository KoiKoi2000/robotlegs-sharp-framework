

using robotlegs.bender.framework.api;
using robotlegs.bender.extensions.contextview.api;
using robotlegs.bender.extensions.viewManager.api;
using robotlegs.bender.extensions.matching;

namespace robotlegs.bender.extensions.contextview
{
	/// <summary>
	/// <p>This Extension class is abstract and should be extended to make a platform specific version.</p>
	/// <p>Installing this extension as it is will cause an error.</p>
	/// 
	/// <p>This Extension will automatically:
	/// <ul>
	/// <li>Initialize the Context when the Context View is added to the stage.</li>
	/// <li>Suspend the Context when the Context View deactiviates</li>
	/// <li>Resume the Context when the Context View re-activates</li>
	/// <li>Suspend the Context when the Context View is destroyed</li>
	/// </ul>
	/// </p>
	/// </summary>


	public abstract class StageSyncExtension : IExtension
	{
		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private IContext _context;
		private IContextView _contextView;
		private IViewStateWatcher _contextViewStateWatcher;

		/*============================================================================*/
		/* Protected Functions                                                           */
		/*============================================================================*/
		protected ILogger _logger;
		
		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public void Extend (IContext context)
		{
			_context = context;
			_logger = context.GetLogger(this);
			
			if(context.injector.HasDirectMapping(typeof(IContextView)))
				HandleContextView(context.injector.GetInstance(typeof(IContextView)));
			else
				context.AddConfigHandler(new InstanceOfMatcher(typeof(IContextView)), HandleContextView);
		}

		/*============================================================================*/
		/* Private Functions                                                           */
		/*============================================================================*/

		private void HandleContextView(object contextView)
		{
			if (_contextView != null)
				return;
			IContextView castContextView = contextView as IContextView;
			if (castContextView == null)
				return;

			_contextView = castContextView;

			if(_contextViewStateWatcher != null)
			{
				_logger.Warn ("A IViewStateWatcher on the context view has already been set");
				return;
			}

			IViewStateWatcher contextViewStateWatcher = GetContextViewStateWatcher(_contextView.view);
			if (contextViewStateWatcher == null)
			{
				_logger.Warn ("A IViewStateWatcher cannot be created on the context view as GetContextViewStateWatcher returned null");
				return;
			}
			_contextViewStateWatcher = contextViewStateWatcher;

			if (contextViewStateWatcher.isAdded)
				InitializeContext ();
			else
				contextViewStateWatcher.added += HandleContextViewAdded;

		}

		private void HandleContextViewAdded(object view)
		{
			_contextViewStateWatcher.added -= HandleContextViewAdded;
			InitializeContext();
		}

		private void InitializeContext()
		{
			_contextViewStateWatcher.removed += HandleContextViewRemoved;
			_contextViewStateWatcher.disabled += HandleContextViewDisabled;
			_context.Initialize();
		}

		private void HandleContextViewDisabled(object view)
		{
			_contextViewStateWatcher.disabled -= HandleContextViewDisabled;
			_contextViewStateWatcher.enabled += HandleContextViewEnabled;
			_context.Suspend();
		}

		private void HandleContextViewEnabled(object view)
		{
			_contextViewStateWatcher.disabled += HandleContextViewDisabled;
			_contextViewStateWatcher.enabled -= HandleContextViewEnabled;
			_context.Resume();
		}		

		private void HandleContextViewRemoved(object view)
		{
			_contextViewStateWatcher.removed -= HandleContextViewRemoved;
			_context.Destroy();	
		}
		
		/*============================================================================*/
		/* Protected Abstract Functions                                               */
		/*============================================================================*/

		protected abstract IViewStateWatcher GetContextViewStateWatcher (object contextView);

	}
}