/**
 * Created by Herb on 9/27/2016.
 */
// TodoStore definition.
// Flux stores house application logic and state that relate to a specific domain.
// In this case, a list of todo items.
import RiotControl from 'riotcontrol';

const baseIdentityServerDeveloperUrlTest = 'http://localhost:31949/api/v1/Developer/';

function IdentityServerDeveloperStore() {
    var self = this
    riot.observable(self) // Riot provides our event emitter.

    self.baseIdentityServerDeveloperUrl = baseIdentityServerDeveloperUrlTest;
    self.scopesUrl = null;
    self.clientsUrl = null;
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

    self.on('developer-api-baseurl', function(baseurl) {
        var triggerName = 'developer-api-baseurl';
        console.log('on',triggerName,baseurl);
        self.baseIdentityServerDeveloperUrl = baseurl;
        self.scopesUrl = self.baseIdentityServerDeveloperUrl + 'scopes';
        self.clientsUrl = self.baseIdentityServerDeveloperUrl + 'clients';
        self.trigger(triggerName+'-ack');
    })
    self.on('developer-clients-page', function(query) {
        var triggerName = 'developer-clients-page';
        console.log('on',triggerName);
        self.pagingState = null;
        var ps = '';
        if(query.PagingState)
            ps = query.PagingState;

        var url = self.clientsUrl + '/page/count/' + query.Page + '/state/' + ps;
        RiotControl.trigger(
            'fetch',url,
            {
                method: 'GET'
            },
            {name:triggerName+'-result'});
    })

    self.on('developer-scopes-get', function(query) {
        var triggerName = 'developer-scopes-get';
        console.log('on',triggerName);
        var url = self.scopesUrl;
        RiotControl.trigger(
            'fetch',url,
            {
                method: 'GET'
            },
            {name:triggerName+'-result'});
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

        console.log('identityserver-admin-users-create:');
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

if (typeof(module) !== 'undefined') module.exports = IdentityServerDeveloperStore;



