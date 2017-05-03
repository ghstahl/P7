 
import './css/index.css';
import './event-helper';

// Put RiotControl first in the startup flow
import RiotControl from 'riotcontrol';
riot.control = RiotControl;
///////////////////////////////////////////////////////////////////////////


// Add the stores
///////////////////////////////////////////////////////////////////////////
import ItemStore 							from './stores/itemstore.js';
RiotControl.addStore(new ItemStore());

import FetchStore                           from './stores/fetch-store.js';
RiotControl.addStore(new FetchStore());


// Add our MixIns
///////////////////////////////////////////////////////////////////////////
import OptsMixin                            from './mixins/opts-mixin.js'
import TestMixin                            from './mixins/test-mixin.js'
import RiotLifeCycleMixin                   from './mixins/riot-lifecycle-mixin.js'
import RiotControlRegistrationMixin         from './mixins/riotcontrol-registration-mixin.js'
import SharedObservableMixin                from './mixins/shared-observable-mixin.js'
import StateInitMixin                       from './mixins/state-init-mixin.js'

riot.mixin("opts-mixin",OptsMixin);
riot.mixin("test-mixin",TestMixin);
riot.mixin("riot-lifecycle-mixin",RiotLifeCycleMixin);
riot.mixin("riotcontrol-registration-mixin",RiotControlRegistrationMixin);
riot.mixin("shared-observable-mixin",SharedObservableMixin);
riot.mixin("state-init-mixin",StateInitMixin);
import './app.tag';
riot.mount('app');

// put Router after we mount the app.
///////////////////////////////////////////////////////////////////////////
import Router 		from './router.js';
riot.router = new Router();

riot.update('app'); 

//riot.update();
// Trigger the first event
///////////////////////////////////////////////////////////////////////////
//riot.control.trigger('final-mount');
//