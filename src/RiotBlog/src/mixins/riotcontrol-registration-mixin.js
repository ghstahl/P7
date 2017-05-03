import RiotControl from 'riotcontrol';

var RiotControlRegistrationMixin  = {

    // init method is a special one which can initialize
    // the mixin when it's loaded to the tag and is not
    // accessible from the tag its mixed in

    // This requires that the riot-lifecycle-mixin is mixed in before this one, as this one requires it to register with it.

    init: function() {
        var self = this;
        console.log('RiotControlRegistrationMixin:init:',self)

        var riotControlMap = []

        self.riotControlRegisterEventHandler = function(evt,handler){
            riotControlMap.push({evt:evt,handler:handler});
            return self;
        }

        self.on('before-mount',function(){
            console.log('RiotControlRegistrationMixin before-mount:',riotControlMap);
            for (var i = 0, len = riotControlMap.length; i < len; i++) {
                RiotControl.on(riotControlMap[i].evt, riotControlMap[i].handler);
            }
        })

        self.on('before-unmount',function(){
            console.log('RiotControlRegistrationMixin before-unmount:',riotControlMap)
            for (var i = 0, len = riotControlMap.length; i < len; i++) {
                RiotControl.off(riotControlMap[i].evt, riotControlMap[i].handler);
            }
        })
    }
}

if (typeof(module) !== 'undefined') module.exports = RiotControlRegistrationMixin;