/**
 * Created by Herb on 9/27/2016.
 */
// TodoStore definition.
// Flux stores house application logic and state that relate to a specific domain.
// In this case, a list of todo items.
import RiotControl from 'riotcontrol';
function RoleStore() {
    riot.observable(this) // Riot provides our event emitter.

    var self = this

    self.roles = []

    self.lastRole = null;

    /**
     * Reset tag attributes to hide the errors and cleaning the results list
     */
    self.resetData = function() {
        self.roles = [];
    }

    // Our store's event handlers / API.
    // This is where we would use AJAX calls to interface with the server.
    // Any number of views can emit actions/events without knowing the specifics of the back-end.
    // This store can easily be swapped for another, while the view components remain untouched.

    self.on('roles_fetch', function() {
        console.log('roles_fetch:');
        self.roles = self.getRoles();
        self.trigger('roles_changed', self.roles)
    })

    self.on('roles_add', function(value) {
        console.log('roles_add:',value);
        self.roles = self.getRoles();

        if(!self.contains(value)){
            self.roles.push(value)
            self.storeRoles(self.roles);
            self.trigger('roles_changed', self.roles)
        }
    })

    self.getRoles = function(){
        var stored = localStorage.getItem('roles');
        if(stored && stored !== "undefined"){
            var restoredSession = JSON.parse(stored)
            return restoredSession;
        }
        return [];
    }

    self.storeRoles = function(roles){
        localStorage.setItem("roles", JSON.stringify(roles));
    }


    self.on('roles_remove', function(value) {
        console.log('roles_remove:',value);
        self.roles = self.getRoles();
        var ll = self.roles.length;
        self.remove(value);
        if(ll != self.roles.length){
            self.storeRoles(self.roles);
            self.trigger('roles_changed', self.roles)
        }
    })

    self.contains = function(value){
        var result = self.roles.filter(function( obj ) {
            return obj == value;
        });
        return result.length > 0;
    }

    self.remove = function(value){
        var index = self.roles.indexOf(value);    // <-- Not supported in <IE9
        if (index !== -1) {
            self.roles.splice(index, 1);
        }
    }
    self.on('roles_clear', function() {
        self.roles.clear();
        self.storeRoles(self.roles);
        self.trigger('roles_changed', self.roles)
    })
    // The store emits change events to any listening views, so that they may react and redraw themselves.

}

if (typeof(module) !== 'undefined') module.exports = RoleStore;



