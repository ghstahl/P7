class ErrorStore{

  constructor(){
    var self = this;
    self.name = 'ErrorStore';
    self.namespace = self.name+':';
    riot.EVT.errorStore ={
        in:{
          errorCatchAll:self.namespace+'error-catch-all'
        },
        out:{
         
        }
    }
    riot.observable(self);
    self._error = {}
    self._bindEvents();
  }
  _bindEvents(){
    var self = this;
    self.on(riot.EVT.errorStore.in.errorCatchAll, (error) => {
      console.log(self.name,riot.EVT.errorStore.in.errorCatchAll,error);
      self._error = error;
      riot.state.error = error;
      riot.control.trigger(riot.EVT.routeStore.in.routeDispatch,'/error');
    });
  }
}
export default ErrorStore;