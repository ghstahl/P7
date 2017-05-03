var TestMixin  = {

    // init method is a special one which can initialize
    // the mixin when it's loaded to the tag and is not
    // accessible from the tag its mixed in

    // This requires that the riot-lifecycle-mixin is mixed in before this one, as this one requires it to register with it.
    init: function() {
        var self = this;
        console.log('TestMixin:init:',self)


    }
}

if (typeof(module) !== 'undefined') module.exports = TestMixin;