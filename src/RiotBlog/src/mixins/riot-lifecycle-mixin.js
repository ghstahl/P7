var RiotLifeCycleMixin  = {

    // init method is a special one which can initialize
    // the mixin when it's loaded to the tag and is not
    // accessible from the tag its mixed in
    init: function() {
        var self=this;
        console.log('RiotLifeCycleMixin:init:', self);
        var _registrants = {
            beforeMount:[],
            mount:[],
            update:[],
            updated:[],
            beforeUnmount:[],
            unmount:[],
        }
        self.registerLifeCycleHandler = function(evtName,handler){
            if(evtName == 'before-mount'){
                _registrants.beforeMount.push(handler);
                return self;
            }
            if(evtName == 'mount'){
                _registrants.mount.push(handler);
                return self;
            }
            if(evtName == 'update'){
                _registrants.update.push(handler);
                return self;
            }
            if(evtName == 'updated'){
                _registrants.updated.push(handler);
                return self;
            }
            if(evtName == 'before-unmount'){
                _registrants.beforeUnmount.push(handler);
                return self;
            }
            if(evtName == 'unmount'){
                _registrants.unmount.push(handler);
                return self;
            }
            return self;
        }

        this.on('before-mount', function() {
            console.log('RiotLifeCycleMixin','before-mount',_registrants)
            for (var i = 0, len = _registrants.beforeMount.length; i < len; i++) {
                _registrants.beforeMount[i]();
            }
        })
        this.on('mount', function() {
            for (var i = 0, len = _registrants.mount.length; i < len; i++) {
                _registrants.mount[i]();
            }
        })
        this.on('update', function() {
            for (var i = 0, len = _registrants.update.length; i < len; i++) {
                _registrants.update[i]();
            }
        })
        this.on('updated', function() {
            for (var i = 0, len = _registrants.updated.length; i < len; i++) {
                _registrants.updated[i]();
            }
        })
        this.on('before-unmount', function() {
            for (var i = 0, len = _registrants.beforeUnmount.length; i < len; i++) {
                _registrants.beforeUnmount[i]();
            }
        })
        this.on('unmount', function() {
            for (var i = 0, len = _registrants.unmount.length; i < len; i++) {
                _registrants.unmount[i]();
            }
        })
    }
}

if (typeof(module) !== 'undefined') module.exports = RiotLifeCycleMixin;
