<header>

<div class="navbar navbar-default navbar-fixed-top">
  <div class="container">
    <div class="navbar-header">
      <a href="../" class="navbar-brand">Bootswatch</a>
      <button class="navbar-toggle" type="button" data-toggle="collapse" data-target="#navbar-main">
        <span class="icon-bar"></span>
        <span class="icon-bar"></span>
        <span class="icon-bar"></span>
      </button>
    </div><!--/.navbar-header -->

    <div class="navbar-collapse collapse" id="navbar-main">
      <ul class="nav navbar-nav">
        <li>
              <a href="https://github.com/ghstahl/riotjs-bootstrap-startkit-sidebar">github</a>
        </li>
      </ul>
      <ul class="nav navbar-nav navbar-right">
        <li each={ navItems } 
          onclick={parent.route}
          class={ active : parent.routeState.route === this.route }>
          <a>{ this.title }</a>
        </li>
      </ul>
    </div><!--/.navbar-collapse collapse -->
  </div><!--/.container -->
</nav>

<script>
  var self = this;
  self.routeState = riot.routeState;

  self.navItems = [
    { title : 'Home', route : '/main/home'},
    { title : 'Projects', route : '/main/projects' }
  ];

  self.on('mount', () => {
    console.log('header mount');
    riot.control.on('riot-route-dispatch-ack',self.onRiotRouteDispatchAck);
  });
  self.on('unmount', () => {
    console.log('header unmount')
    riot.control.off('riot-route-dispatch-ack',self.onRiotRouteDispatchAck);
  });

  self.onRiotRouteDispatchAck = () =>{
    console.log('header riot-route-dispatch-ack')
    self.update()
  }

  self.route = (evt) => {
    riot.control.trigger('riot-route-dispatch',evt.item.route);
  };

</script>

</header>
