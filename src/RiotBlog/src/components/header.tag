<header>

<button 
  each={ navItems } 
  onclick={parent.route} 
  type="button" 
  class = {parent.routeState.view === this.view ? 'active btn btn-space btn-outline-primary' : 'btn btn-space btn-outline-primary'}
  >{ this.title }</button>

<script>

  var self = this;
  self.routeState = riot.routeState

  self.navItems = [
    { title : 'Home', view : 'home'},
    { title : 'Projects', view : 'projects' }
  ];

  self.route = (evt) => {
    riot.route(evt.item.view)
  };

</script>

</header>
