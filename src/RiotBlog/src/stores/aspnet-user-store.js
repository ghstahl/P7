/**
 * Created by Herb on 9/27/2016.
 */
// TodoStore definition.
// Flux stores house application logic and state that relate to a specific domain.
// In this case, a list of todo items.
import RiotControl from 'riotcontrol';

const baseAspNetUrlTest = 'http://localhost:31949/api/v1/IdentityAdmin2/';
const baseIdentityServerAdminUrlTest = 'http://localhost:31949/api/v1/IdentityServerAdmin/';

function AspNetUserStore() {
    var self = this

    riot.observable(self) // Riot provides our event emitter.

    self.fetchException = null;
    self.baseAspNetUrl = baseAspNetUrlTest;
    self.baseIdentityServerAdminUrl = baseIdentityServerAdminUrlTest;
    /**
     * Reset tag attributes to hide the errors and cleaning the results list
     */
    self.resetData = function() {
        self.fetchException = null;

    }

    self.on('app-mount', function() {
        console.log('AspNetUserStore app-mount');

    })
    self.on('app-unmount', function() {
        console.log('AspNetUserStore app-unmount');
    })

    self.on('aspnet-api-baseurl', function(baseurl) {
        console.log('aspnet-api-baseurl:',baseurl);
        self.baseAspNetUrl = baseurl;
        self.trigger('aspnet-api-baseurl-ack');
    })

    self.on('identityserver-api-baseurl', function(baseurl) {
        console.log('identityserver-api-baseurl:',baseurl);
        self.baseIdentityServerAdminUrl = baseurl;
        self.trigger('identityserver-api-baseurl-ack');
    })

    self.on('aspnet_users_page', function(query) {
        console.log('aspnet_users_page:');
        self.pagingState = null;
        var ps = '';
        if(query.PagingState)
            ps = query.PagingState;
        var url = self.baseAspNetUrl + 'users/page/count/' + query.Page + '/state/' + ps;
        RiotControl.trigger('fetch',url,null,{name:'aspnet_users_page_changed'});
    })
    self.on('aspnet_user_by_id', function(query) {
        console.log('aspnet_user_by_id:');
        var url = self.baseAspNetUrl + 'users/id/' + query.id;
        RiotControl.trigger('fetch',url,null,{name:'aspnet_user_changed'});
    })

    self.on('aspnet_roles_fetch', function() {
        console.log('aspnet_roles_fetch:');
        var url = self.baseAspNetUrl + 'roles';
        RiotControl.trigger('fetch',url,null,{name:'aspnet_roles_changed'});
    })

    self.on('aspnet_roles_create', function(query) {
        console.log('aspnet_roles_create:');
        var url = self.baseAspNetUrl + 'roles';
        RiotControl.trigger(
            'fetch',
            url,
            {
                method: 'POST',
                body: {
                    role: query.role
                }
            },
            {name:'aspnet_roles_create_ack'});
    })

    

    self.on('aspnet_roles_delete', function(query) {
        console.log('aspnet_roles_delete:');
        var url = self.baseAspNetUrl + 'roles/name/' + query.role;
        RiotControl.trigger(
            'fetch',
            url,
            {
                method: 'DELETE'
            },
            {name:'aspnet_roles_delete_ack'});
    })


    // Our store's event handlers / API.
    // This is where we would use AJAX calls to interface with the server.
    // Any number of views can emit actions/events without knowing the specifics of the back-end.
    // This store can easily be swapped for another, while the view components remain untouched.


    self.on('aspnet_user_roles_add', function(query) {
        console.log('aspnet_user_roles_add:');
        var url = self.baseAspNetUrl + 'users/id/' + query.id +'/roles/name/' + query.role;
        RiotControl.trigger(
            'fetch',
            url,
            {
                method: 'POST'
            },
            {
                name:'riot-trigger',
                query:{
                    evt:'aspnet_user_by_id',
                    query:{
                        id:query.id
                    }
                }
            });
    })

    self.on('aspnet_user_role_remove', function(query) {
        console.log('aspnet_user_role_remove:');
        var url = self.baseAspNetUrl + 'users/id/' + query.id +'/roles/name/' + query.role;
        RiotControl.trigger(
            'fetch',
            url,
            {
                method: 'DELETE'
            },
            {
                name:'riot-trigger',
                query:{
                    evt:'aspnet_user_by_id',
                    query:{
                        id:query.id
                    }
                }
            });
    })

}

if (typeof(module) !== 'undefined') module.exports = AspNetUserStore;



