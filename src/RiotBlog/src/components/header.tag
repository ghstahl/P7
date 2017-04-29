<header>
header
<button 
  each={ navItems } 
  onclick={parent.route} 
  type="button" 
  class = {parent.routeState.view === this.view ? 'active btn btn-space btn-outline-primary' : 'btn btn-space btn-outline-primary'}>
    {this.title} {parent.routeState.view} {parent.currentView} {this.view}

</button>

<script>
  var self = this;
  self.cc2 = {};
  self.cc3 = {};
 
  self.routeState = riot.routeState;
  self.currentView = riot.routeState.view;
  self.navItems = [
    { title : 'Home', view : 'home'},
    { title : 'Projects', view : 'projects' }
  ];

  self.route = (evt) => {
    riot.route(evt.item.view);
 //   self.currentView = evt.item.view;
    self.cc2 = self.routeState.view;
    self.cc3 = self.cc2;
  };

</script>

</header>
