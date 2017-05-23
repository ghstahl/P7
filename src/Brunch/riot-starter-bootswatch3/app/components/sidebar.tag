<sidebar>
 
		<a 	each={ state.items } 
			onclick={parent.route}
			class={ parent.routeState.route === this.route? 'active list-group-item':'list-group-item'  } 
			>{this.title}</a>
	 
</div>        
 

<script>
	var self = this;
	self.state = riot.state.sidebar;
  	self.routeState = riot.routeState;

	self.on('mount', () => {
	    console.log('sidebar mount');
	    riot.control.on('riot-route-dispatch-ack',self.onRiotRouteDispatchAck);
	  });
	  self.on('unmount', () => {
	    console.log('sidebar unmount')
	    riot.control.off('riot-route-dispatch-ack',self.onRiotRouteDispatchAck);
	  });

	  self.onRiotRouteDispatchAck = () =>{
	    console.log('sidebar riot-route-dispatch-ack')
	    self.update()
	  }

	  self.route = (evt) => {
		riot.control.trigger('riot-route-dispatch',evt.item.route);
	  };
	  
	</script>
</sidebar>