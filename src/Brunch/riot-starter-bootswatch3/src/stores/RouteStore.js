class RouteStore{

  constructor(){
    var self = this;
    self.name = "RouteStore";
    self.namespace = self.name+':';
    riot.EVT.routeStore ={
        in:{
          routeCatchallReset:'route-catchall-reset',
          routeDispatch:'riot-route-dispatch'
        },
        out:{
          riotRouteDispatchAck:'riot-route-dispatch-ack'
        }
    }

    riot.observable(self);
    self.bindEvents();
    self.postResetRoute = null;
  }

 
  bindEvents(){
    var self = this;

    self.on(riot.EVT.contributeCatchAllRoute, (r) => {
      console.log(self.name,riot.EVT.contributeRoutes,r)
      if(riot.state.componentLoaderState && riot.state.componentLoaderState.components){
        for(let item of riot.state.componentLoaderState.components){
          var component = item[1];
          if(component.state.loaded == false){
      			r( component.routeLoad.route,()=>{
      			        console.log('catchall route handler of:',component.routeLoad.route,path )
                    var q = riot.route.query();
                    var path = riot.route.currentPath();
                    self.postResetRoute = path;
                    riot.control.trigger('load-dynamic-component',component.key);  
                  }) 
          }
        }
      }
      r( ()=>{
        console.log('route handler of /  ' )
        riot.control.trigger(riot.EVT.routeStore.in.routeDispatch,riot.state.route.defaultRoute);
      }) 
      if(self.postResetRoute != null){
        var postResetRoute = self.postResetRoute;
        self.postResetRoute = null;
        riot.control.trigger('riot-route-dispatch',postResetRoute,true);
      }

    });

    

    self.on(riot.EVT.routeStore.in.routeDispatch, (route, force) => {
      console.log(self.name,riot.EVT.routeStore.in.routeDispatch,route)
      var current =  riot.route.currentPath();
      
      var same = current == route;
        if(!same){
          same = ('/' +current) == route;
        }
        if (!same) {
          riot.route(route);
        } else {
          if (force) {
            riot.route.exec();
          }
        }
     
      riot.routeState.route = route;
      self.trigger(riot.EVT.routeStore.in.routeDispatchAck, route);
    });

    self.on(riot.EVT.routeStore.in.routeCatchallReset, () => {
      console.log(self.name,riot.EVT.routeStore.in.routeCatchallReset)
      riot.router.resetCatchAll();
    });

    self.on('riot-route-add-view', (view) => {
      console.log(self.name,'riot-route-add-view',view)
      var s = self._viewsSet;
      s.add(view);
      riot.routeState.views = Array.from(s);
    });

    self.on('riot-route-remove-view', (view) => {
      console.log(self.name,'riot-route-remove-view',view)
      var s = self._viewsSet;
      s.delete(view);
      riot.routeState.views = Array.from(s);
    });
    self.on('riot-route-load-view', (view) => {
      console.log(self.name,'riot-route-load-view',view)
      riot.router.loadView(view);
    });
  }
}
export default RouteStore;