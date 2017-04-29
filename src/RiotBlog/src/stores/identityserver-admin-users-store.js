/**
 * Created by Herb on 9/27/2016.
 */
// TodoStore definition.
// Flux stores house application logic and state that relate to a specific domain.
// In this case, a list of todo items.
import RiotControl from 'riotcontrol';

const baseIdentityServerAdminUrlTest = 'http://localhost:31949/api/v1/IdentityServerAdmin/';

function IdentityServerAdminUsersStore() {
    var self = this
    riot.observable(self) // Riot provides our event emitter.

    self.baseIdentityServerAdminUrl = baseIdentityServerAdminUrlTest;
    self.usersUrl = null;
    /**
     * Reset tag attributes to hide the errors and cleaning the results list
     */
    self.resetData = function() {

    }


    self.on('app-mount', function() {
        console.log('IdentityServerAdminUsersStore app-mount');
    })

    self.on('app-unmount', function() {
        console.log('IdentityServerAdminUsersStore app-unmount');
    })


    self.on('identityserver-api-baseurl', function(baseurl) {
        var triggerName = 'identityserver-api-baseurl';
        console.log('on',triggerName,baseurl);
        self.baseIdentityServerAdminUrl = baseurl;
        self.usersUrl = self.baseIdentityServerAdminUrl + 'users';
        self.trigger(triggerName+'-ack');
    })

    self.on('identityserver-admin-users-get', function(query) {
        var triggerName = 'identityserver-admin-users-get';
        console.log('on',triggerName);
        var url = self.usersUrl + '/id/' + query.userId;
        RiotControl.trigger(
            'fetch',url,
            {
                method: 'GET'
            },
            {name:triggerName+'-result',query:query});
    })

    self.on('identityserver-admin-users-delete', function(query) {
        var triggerName = 'identityserver-admin-users-delete';
        console.log('on',triggerName);
        var url = self.usersUrl + '/id/' +query.userId;
        RiotControl.trigger(
            'fetch',url,
            {
                method: 'DELETE'
            },
            {name:triggerName+'-ack',query:query});
    })

    self.on('identityserver-admin-users-create', function(query) {
        var triggerName = 'identityserver-admin-users-create';
        console.log('on',triggerName);

        var url = self.usersUrl;

        RiotControl.trigger(
            'fetch',url,
            {
                method: 'POST',
                body: {
                    userId:query.userId
                }
            },
            {name:triggerName+'-ack',query:query});
    })
}

if (typeof(module) !== 'undefined') module.exports = IdentityServerAdminUsersStore;



