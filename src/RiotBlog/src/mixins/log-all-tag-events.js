var LogAllTagEventsMixin = {
    self : this, // not yet
    // init method is a special one which can initialize
    // the mixin when it's loaded to the tag and is not
    // accessible from the tag its mixed in
    init: function() {
        console.log('LogAllTagEventsMixin',this)
        this.on('all', function(evt) { console.log('......LogAll:',evt) })
    },


    getOpts: function() {
        return this.opts
    },

    setOpts: function(opts, update) {
        this.opts = opts
        if (!update) this.update()
        return this
    }
}

if (typeof(module) !== 'undefined') module.exports = LogAllTagEventsMixin;
