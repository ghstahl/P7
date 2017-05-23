
class Router{

  constructor(){
    var self = this;
    self.name = 'Router';
    self.namespace = self.name+':';
    
    // we need this to easily check the current route from every component
    riot.routeState.view  = '';
    self.r = riot.route.create()
    self.resetCatchAll(); 
  }
  
  resetCatchAll(){
    var self = this;
    self.r.stop();
    riot.control.trigger(riot.EVT.router.out.contributeRoutes,self.r);
    riot.control.trigger(riot.EVT.router.out.contributeCatchAllRoute,self.r);

  }

  loadView(view){
    var self = this;
    if (self._currentView) {
      self._currentView.unmount(true);
    }

    riot.routeState.view = view;
    self._currentView = riot.mount('#riot-app', view)[0];
  }

}
export default Router;
 