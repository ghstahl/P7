var SharedObservableMixin  = {
    observable: riot.observable(),
    // init method is a special one which can initialize
    // the mixin when it's loaded to the tag and is not
    // accessible from the tag its mixed in

    // This requires that the riot-lifecycle-mixin is mixed in before this one, as this one requires it to register with it.
    init: function() {
        var self = this;
        console.log('SharedObservableMixin:init:',self)
        self.triggerEvent = (evt,args) =>{
            args.unshift(evt);
            self.observable.trigger.apply(self,args);
        }

        console.log('RiotControlRegistrationMixin:init:',self)

        var observerableMap = []

        self.registerObserverableEventHandler = function(evt,handler){
            observerableMap.push({evt:evt,handler:handler});
            return self;
        }

        self.on('before-mount',function(){
            console.log('SharedObservableMixin before-mount:',observerableMap);
            for (var i = 0, len = observerableMap.length; i < len; i++) {
                self.observable.on(observerableMap[i].evt, observerableMap[i].handler);
            }
        })

        self.on('before-unmount',function(){
            console.log('SharedObservableMixin before-unmount:',observerableMap)
            for (var i = 0, len = observerableMap.length; i < len; i++) {
                self.observable.off(observerableMap[i].evt, observerableMap[i].handler);
            }
        })
    }
}

if (typeof(module) !== 'undefined') module.exports = SharedObservableMixin;


