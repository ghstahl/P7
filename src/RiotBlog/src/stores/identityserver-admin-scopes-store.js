/**
 * Created by Herb on 9/27/2016.
 */
// TodoStore definition.
// Flux stores house application logic and state that relate to a specific domain.
// In this case, a list of todo items.
import RiotControl from 'riotcontrol';

const baseIdentityServerAdminUrlTest = 'http://localhost:31949/api/v1/IdentityServerAdmin/';

function IdentityServerAdminScopesStore() {
    var self = this
    riot.observable(self) // Riot provides our event emitter.

    self.baseIdentityServerAdminUrl = baseIdentityServerAdminUrlTest;
    self.scopesUrl = null;
    /**
     * Reset tag attributes to hide the errors and cleaning the results list
     */
    self.resetData = function() {

    }


    self.on('app-mount', function() {
        console.log('IdentityServerAdminScopesStore app-mount');
    })

    self.on('app-unmount', function() {
        console.log('IdentityServerAdminScopesStore app-unmount');
    })


    self.on('identityserver-api-baseurl', function(baseurl) {
        var triggerName = 'identityserver-api-baseurl';
        console.log('on',triggerName,baseurl);
        self.baseIdentityServerAdminUrl = baseurl;
        self.scopesUrl = self.baseIdentityServerAdminUrl + 'scopes';
        self.trigger(triggerName+'-ack');
    })

    self.on('identityserver-admin-scopes-get', function() {
        var triggerName = 'identityserver-admin-scopes-get';
        console.log('on',triggerName);
        var url = self.scopesUrl;
        RiotControl.trigger(
            'fetch',url,
            {
                method: 'GET'
            },
            {name:triggerName +'-result'});
    })

    self.on('identityserver-admin-scopes-delete', function(query) {
        var triggerName = 'identityserver-admin-scopes-delete';
        console.log('on',triggerName);

        var url = self.scopesUrl + '/name/' + query.name;
        RiotControl.trigger(
            'fetch',url,
            {
                method: 'DELETE'
            },
            {
                name:'riot-trigger',
                query:{
                    evt:'identityserver-admin-scopes-get'
                }
            });
    })



    self.on('identityserver-admin-scopes-create', function(query) {
        var triggerName = 'identityserver-admin-scopes-create';
        console.log('on',triggerName);

        var url = self.scopesUrl;
        RiotControl.trigger(
            'fetch',url,
            {
                method: 'POST',
                body: {
                    name:query.name,
                    type:query.type,
                    enabled:query.enabled
                }
            },
            {
                name:'riot-trigger',
                query:{
                    evt:'identityserver-admin-scopes-get'
                }
            } );
    })
}

if (typeof(module) !== 'undefined') module.exports = IdentityServerAdminScopesStore;



