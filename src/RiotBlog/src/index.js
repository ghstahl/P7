 
import './css/index.css';
import './event-helper';


// Put RiotControl first in the startup flow
import RiotControl from 'riotcontrol';
riot.control = RiotControl;
import './app.tag';

// Add the stores

import ItemStore 	from './stores/itemstore.js';
riot.control.addStore(new ItemStore());

riot.mount('app');
// put Router Last
import Router 		from './router.js';
riot.router = new Router();

riot.control.trigger('final-mount');

