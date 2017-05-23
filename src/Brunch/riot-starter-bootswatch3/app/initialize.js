import route from 'riot-route';
riot.route = route;
import RiotControl from 'riotcontrol';
riot.control = RiotControl;
import RiotRouteExtension            	from '../src/extensions/riot-route-extension.js';
new RiotRouteExtension();


// Add the mixings
////////////////////////////////////////////////////////
import OptsMixin                            from '../src/mixins/opts-mixin.js'
riot.mixin("opts-mixin",OptsMixin);
// Add the stores
////////////////////////////////////////////////////////
import RiotControlDispatcherStore 	from '../src/stores/RiotControlDispatcherStore.js';
riot.control.addStore(new RiotControlDispatcherStore());

import ProgressStore            	from '../src/stores/progress-store.js';
riot.control.addStore(new ProgressStore());

import LocalStorageStore         	from '../src/stores/localstorage-store.js';
riot.control.addStore(new LocalStorageStore());

import RiotControlStore 			from '../src/stores/RiotControlStore.js';
riot.control.addStore(new RiotControlStore());

import FetchStore 					from '../src/stores/fetch-store.js';
riot.control.addStore(new FetchStore());

import RouteStore 					from '../src/stores/RouteStore.js';
riot.control.addStore(new RouteStore());

import DynamicJsCssLoaderStore 		from '../src/stores/dynamic-jscss-loader-store.js';
riot.control.addStore(new DynamicJsCssLoaderStore());

import PluginRegistrationStore 		from '../src/stores/plugin-registration-store.js';
riot.control.addStore(new PluginRegistrationStore());

import ComponentLoaderStore 		from '../src/stores/component-loader-store.js';
riot.control.addStore(new ComponentLoaderStore());

import StartupStore 				from '../src/stores/startup-store.js';
riot.control.addStore(new StartupStore());

import ErrorStore            		from './stores/error-store.js';
riot.control.addStore(new ErrorStore());

import RouteContributionStore 		from './stores/route-contribution-store.js';
riot.control.addStore(new RouteContributionStore());


riot.routeState = {};
riot.state = {
	error:{code:'unknown'},
	route:{
		defaultRoute:'/main/home/'
	},
	sidebar:{
		touch:0,
		items:[
			{ title : 'Home', route : '/main/home'},
			{ title : 'Projects', route : '/main/projects'}
		]
	}
};

document.addEventListener('DOMContentLoaded', () => {
    // do your setup here
    console.log('Initialized app');
});

