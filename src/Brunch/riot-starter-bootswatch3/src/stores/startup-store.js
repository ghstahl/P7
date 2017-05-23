import Router     from '../router.js';

class StartupStore{

  constructor(){
    var self = this;
    self.name = 'StartupStore';
    self.namespace = self.name +':';
    riot.EVT.startupStore ={
        in:{
          start:self.namespace + 'start',
          fetchConfig:self.namespace + 'fetch-config',
          fetchConfigResult:self.namespace + 'fetch-config-result',
          fetchConfigResult2:self.namespace + 'fetch-config-result2',
          componentsAdded:self.namespace + 'components-added',
          allComponentsLoadComplete:riot.EVT.componentLoaderStore.out.allComponentsLoadComplete
        },
        out:{
          routeCatchallReset:riot.EVT.routeStore.in.routeCatchallReset
        }
    }

    self._startupComplete = false;
    riot.observable(self);
    self._bindEvents();
  }
  _bindEvents(){
    var self = this;
    
    //------------------------------------------------------------
    self.on(riot.EVT.startupStore.in.start, () => {
      console.log(self.name,riot.EVT.startupStore.in.start);
      riot.mount('app');
      riot.router = new Router();
      riot.route.start(true);
    });
     
    //------------------------------------------------------------
    self.on(riot.EVT.startupStore.in.fetchConfig, (path,ack) => {
      console.log(self.name,riot.EVT.startupStore.in.fetchConfig,path);
      var url = path;
      var trigger = {
        name:riot.EVT.startupStore.in.fetchConfigResult,
        ack:ack
      };
      var trigger2 = {
        name:riot.EVT.startupStore.in.fetchConfigResult2,
        ack:ack
      };
      riot.control.trigger(riot.EVT.fetchStore.in.fetch,url,{method:'HEAD'},trigger2);
      riot.control.trigger(riot.EVT.fetchStore.in.fetch,url,null,trigger);
    });



    //------------------------------------------------------------
    self.on(riot.EVT.startupStore.in.fetchConfigResult2, (result,myTrigger) => {
      console.log(self.name,riot.EVT.startupStore.in.fetchConfigResult2,result,myTrigger);
       
    });

    //------------------------------------------------------------
    self.on(riot.EVT.startupStore.in.fetchConfigResult, (result,myTrigger) => {
      console.log(self.name,riot.EVT.startupStore.in.fetchConfigResult,result,myTrigger);
      if(result.error || !result.response.ok){
        riot.control.trigger('ErrorStore:error-catch-all',{code:'startup-config1234'});
      }else{
        riot.control.trigger(riot.EVT.componentLoaderStore.in.addDynamicComponents
          ,result.json.components,{evt:riot.EVT.startupStore.in.componentsAdded,
            ack:myTrigger.ack});
      }
    });

    //------------------------------------------------------------
    self.on(riot.EVT.startupStore.in.componentsAdded, (ack) => {
      console.log(self.name,riot.EVT.startupStore.in.componentsAdded,ack);
      riot.control.trigger(ack.ack.evt,ack.ack);

    });

    // this is the one and only handler when components are added and loaded.
    // it is meant to trigger a route rebuild.
    self.onAllComponentsLoadComplete = () =>{
      console.log(self.name,riot.EVT.startupStore.in.allComponentsLoadComplete);
      riot.control.trigger(riot.EVT.startupStore.out.routeCatchallReset);
    }
    //------------------------------------------------------------
    self.on(riot.EVT.startupStore.in.allComponentsLoadComplete, 
      self.onAllComponentsLoadComplete);
  }
}
export default StartupStore;