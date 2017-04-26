<header>

<button 
  each={ navItems } 
  onclick={parent.route} 
  type="button" 
  class = {parent.currentView === this.view ? 'active btn btn-space btn-outline-primary' : 'btn btn-space btn-outline-primary'}
  >{ this.title }</button>

<script>

  this.currentView = riot.routeState.view;

  this.navItems = [
    { title : 'Home', view : 'home'},
    { title : 'Projects', view : 'projects' }
  ];

  this.route = (evt) => {
    riot.route(evt.item.view)
  };

</script>

</header>
