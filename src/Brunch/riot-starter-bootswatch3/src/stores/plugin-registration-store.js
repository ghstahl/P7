
/*
var registerRecord = {
  name:'riotjs-partial-spa',
  views:[
    {view:'my-component-page'},
    {view:'typicode-user-detail'}
  ],
  stores:[
    {store: new TypicodeUserStore()}
  ],
  postLoadEvents:[
    {event:'typicode-init',data:{}}
  ],
  preUnloadEvents:[
    {event:'typicode-uninit',data:{}}
  ]
};
riot.control.trigger('plugin-registration',registerRecord);

*/
class PluginRegistrationStore{

  constructor(){
    var self = this;
    riot.observable(self);
    self.bindEvents();
    self._registeredPlugins = new Set();
  }
  bindEvents(){
    var self = this;
    self.on('plugin-registration', self._registerPlugin);
    self.on('plugin-unregistration', self._unregisterPlugin);
  }

  _findRegistration(registrationName){
    var self = this;
    var mySet = self._registeredPlugins;
    for (let item of mySet) {
        if(item.name === registrationName)
          return item;
    }
    return null;
  }
  _removeRegistration(registrationName){
    var self = this;
    var mySet = self._registeredPlugins;
    for (let item of mySet) {
        if(item.name === registrationName){
          mySet.delete(item);
          break;
        }
    }
    return null;
  }
  _unregisterPlugin(registration){
    var self = this;
    var foundRegistration = self._findRegistration(registration.name);
    if(foundRegistration === null){
      self.trigger('plugin-unregistration-ack', 
        {
          state:false,
          registration:registration,
          error:'plugin already unregistered!'
        });
    }else{
      // reverse unload
      // 1. PreUnload Events first
      for(var i=0; i<foundRegistration.preUnloadEvents.length; i++) {
        riot.control.trigger(foundRegistration.preUnloadEvents[i].event,foundRegistration.preUnloadEvents[i].data);
      }
      // 2. Remove the stores.
      for(var i=0; i<foundRegistration.stores.length; i++) {
        riot.control.trigger(riot.EVT.riotControlStore.in.riotContolRemoveStore,foundRegistration.stores[i].name);
      }

      self._removeRegistration(registration.name);
      self.trigger('plugin-unregistration-ack', 
        {
          state:true,
          registration:registration
        });
    }
  }

  _registerPlugin(registration){
    var self = this;
    var foundRegistration = self._findRegistration(registration.name);
   
    if(foundRegistration === null){
      self._registeredPlugins.add(registration);

      // 1. Add the stores
      for(var i=0; i<registration.stores.length; i++) {
        registration.stores[i].name = registration.name + '-store-' + i; // need this for my own tracking
        riot.control.trigger(riot.EVT.riotControlStore.in.riotContolAddStore,registration.stores[i].name,registration.stores[i].store);
      }
      // 2. fire post load events
      for(var i=0; i<registration.postLoadEvents.length; i++) {
        riot.control.trigger(registration.postLoadEvents[i].event,registration.postLoadEvents[i].data);
      }
      self.trigger('plugin-registration-ack', {state:true,registration:registration});
    }else{
      self.trigger('plugin-registration-ack', {state:false,registration:registration,error:'plugin already registered!'});
    }
  }
}
export default PluginRegistrationStore;