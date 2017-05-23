import "../components/pretty-json.tag"

<projects>


<div each={ component in components }   class="panel panel-default">
	
	
  <div class="panel-heading">
    <h3 class="panel-title">{component.key}</h3>
  </div>

  <div class="panel-body">
    <div class="well well-lg">  
		This will load a mini spa which has been pre-bundled.
		This mini spa was built using the riotjs-partial-tag nested project.
	</div>
	

	<a 	onclick={parent.loadMyComponentsSPA}  
		class={component.state.loaded == false?'btn btn-default btn-lg':'disabled btn btn-default btn-lg'}>
		Load Component</a>

	<a 	onclick={parent.unloadMyComponentsSPA} 
		class={component.state.loaded == true?'btn btn-default btn-lg':'disabled btn btn-default btn-lg'}>
		Unload Component</a>

	<a 	onclick={this.clearLocalStorage} 
		class='btn btn-primary btn-lg'>
		Clear Local Storage</a>

	<div class="spacer"></div>
	<pretty-json obj={component}></pretty-json>
  </div>
</div>



<script>
	var self = this;
	self.mixin("opts-mixin");
	self.name = 'projects';
	
	self.on('before-mount', () => {
		if(riot.state.projects === undefined){
			riot.state.projects = {loaded:false, text:"Not Loaded Yet..."}
		}
		 
		if(riot.state.componentLoaderState != null &&
			riot.state.componentLoaderState.components != null){
			self.components = self.getComponentsArray();
		}
	   
	  });

	self.getComponentsArray = () =>{
		var result = [];
		riot.state.componentLoaderState.components.forEach((value, key, map)=>{
			result.push(value);
		});
		return result;
	} 
	 
	self.on('mount', () => {
    	console.log(self.name,'mount')
      	riot.control.on('component-loader-store-state-updated',
      		self.onComponentLoaderStoreStateUpdated);
    });
    
    self.on('unmount', () => {
 		console.log(self.name,'unmount')
      	riot.control.off('component-loader-store-state-updated',self.onComponentLoaderStoreStateUpdated);
    });
	
	self.onComponentLoaderStoreStateUpdated = () => {
		console.log(self.name,'component-loader-store-state-updated')
  		if(riot.state.componentLoaderState != null && 
  			riot.state.componentLoaderState.components != null){
			self.components = self.getComponentsArray();
		}
  	};

  	self.clearLocalStorage = () => {
  		riot.control.trigger(riot.EVT.localStorageStore.in.localstorageClear);
  	};
	self.loadMyComponentsSPA = (e) => {
		var component = e.item.component;
		var key = component.key;
  		riot.control.trigger('load-dynamic-component',key);
  	};

  	self.unloadMyComponentsSPA = (e) => {
		var component = e.item.component;
  		var key = component.key;
  		riot.control.trigger('unload-dynamic-component',key);
  	};
</script>

</projects>
