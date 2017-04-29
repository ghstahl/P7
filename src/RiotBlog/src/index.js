 
import './css/index.css';

import RiotControl from 'riotcontrol';

import './js/ba-tiny-pubsub.js';
import './event-helper';
import './app.tag';

import ProgressStore                        from './stores/progress-store.js';
import FetchStore                           from './stores/fetch-store.js';
import LocalStorageItemsStore               from './stores/localstorage-items-store.js';
import LocalStorageStore                    from './stores/localstorage-store.js';
import itemstore 							from './stores/itemstore';


import OptsMixin                            from './mixins/opts-mixin.js'
import TestMixin                            from './mixins/test-mixin.js'
import RiotLifeCycleMixin                   from './mixins/riot-lifecycle-mixin.js'
import RiotControlRegistrationMixin         from './mixins/riotcontrol-registration-mixin.js'
import SharedObservableMixin                from './mixins/shared-observable-mixin.js'
import StateInitMixin                       from './mixins/state-init-mixin.js'


RiotControl.addStore(new LocalStorageItemsStore("client-credential"))
RiotControl.addStore(new LocalStorageItemsStore("role"))
RiotControl.addStore(new LocalStorageStore())
RiotControl.addStore(new ProgressStore())
RiotControl.addStore(new FetchStore())

riot.mixin("opts-mixin",OptsMixin);
riot.mixin("test-mixin",TestMixin);
riot.mixin("riot-lifecycle-mixin",RiotLifeCycleMixin);
riot.mixin("riotcontrol-registration-mixin",RiotControlRegistrationMixin);
riot.mixin("shared-observable-mixin",SharedObservableMixin);
riot.mixin("state-init-mixin",StateInitMixin);


riot.mount('app');
import './router.js';