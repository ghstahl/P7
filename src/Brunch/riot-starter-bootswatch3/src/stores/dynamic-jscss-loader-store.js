// http://www.javascriptkit.com/javatutors/loadjavascriptcss.shtml

/*
component:{
		key:'typicode-component',
		path:'/partial/bundle.js',
		type:'js'
	}
	or when unloading
component:{
		key:'typicode-component'
	}

events:{
	out:[
		{
			event:'load-external-jscss-ack',
			type:'riotcontrol'
			data:[
				{
			    	state:true,
			    	component:component
				},
				{
			    	state:false,
			    	component:component,
			    	error:"component already added!"
				}
			]
		},
		{
			event:'unload-external-jscss-ack',
			type:'riotcontrol'
			data:[
				{
			    	state:true,
			    	component:component
				},
				{
			    	state:false,
			    	component:component,
			    	error:"no entry found to remove!"
				}
			]
		}

	]

}
	 
	 
	*/
class DynamicJsCssLoaderStore{
	constructor(){
		var self = this;
		self.name = 'DynamicJsCssLoaderStore';
		self.namespace = self.name + ':';
		riot.EVT.dynamicJsCssLoaderStore ={
	        in:{
	        	loadExternalJsCss:self.namespace + 'load-external-jscss',
	        	unloadExternalJsCss:self.namespace + 'unload-external-jscss'
	        },
	        out:{
	        	loadExternalJsCssAck:self.namespace + 'load-external-jscss-ack',
	        	unloadExternalJsCssAck:self.namespace + 'unload-external-jscss-ack'
	        }
	    }
		riot.observable(this);
		this._bindEvents();
		this._componentsAddedSet = new Set();
	}


	_addComponent(component){
		if(this._findComponent(component) == null){
			var mySet = this._componentsAddedSet;
			mySet.add(component)
			
		}
	}

	_findComponent(component){
	    var mySet = this._componentsAddedSet;
	    for (let item of mySet) {
	        if(item.key === component.key)
	          return item;
	    }
	    return null;
	  }

	_deleteComponent(component){
	    var mySet = this._componentsAddedSet;
	    for (let item of mySet) {
	        if(item.key === component.key){
	          mySet.delete(item);
	        	break;
	        }
	    }
	  }

	_safeLoadExternal(component){
		var addedCompoment = this._findComponent(component);
		if(addedCompoment == null){
			this._loadExternal(component);
			this._addComponent(component);
		    console.log('load-external-jscss',component);
		    riot.control.trigger(riot.EVT.dynamicJsCssLoaderStore.out.loadExternalJsCssAck, 
		    	{state:true,component:component});
	    }
	    else{
	    	console.error("file already added!",component);
		    riot.control.trigger(riot.EVT.dynamicJsCssLoaderStore.out.loadExternalJsCssAck, {
		    	state:false,
		    	component:component,
		    	error:"component already added!"});
	    }
	}
	_removeExternal(component){
		var addedCompoment = this._findComponent(component);
		if(addedCompoment == null){
			riot.control.trigger(riot.EVT.dynamicJsCssLoaderStore.out.unloadExternalJsCssAck, {
		    	state:false,
		    	component:component,
		    	error:"no entry found to remove!",});
		}else{
			var filename = component.path;
			var filetype = component.type;
			var targetelement=(filetype=="js")? "script" : (filetype=="css")? "link" : "none" //determine element type to create nodelist from
	    	var targetattr=(filetype=="js")? "src" : (filetype=="css")? "href" : "none" //determine corresponding attribute to test for
	    	var allsuspects=document.getElementsByTagName(targetelement)
	    	for (var i=allsuspects.length; i>=0; i--){ //search backwards within nodelist for matching elements to remove
			    if (	allsuspects[i] 
			    	&& 	allsuspects[i].getAttribute(targetattr)!=null 
			    	&& 	allsuspects[i].getAttribute(targetattr).indexOf(filename)!=-1){
			    	allsuspects[i].parentNode.removeChild(allsuspects[i]) //remove element by calling parentNode.removeChild()
					this._deleteComponent(component);
					
					riot.control.trigger(riot.EVT.dynamicJsCssLoaderStore.out.unloadExternalJsCssAck, {
				    	state:true,
				    	component:component});
					break;
		    	}     
		    }
		}
	}

	_loadExternal(component){
		var filename = component.path;
		var filetype = component.type;
		if (filetype=="js"){ //if filename is a external JavaScript file
	        var fileref=document.createElement('script');
	        fileref.setAttribute("type","text/javascript");
	        fileref.setAttribute("src", filename);
	    }
	    else if (filetype=="css"){ //if filename is an external CSS file
	        var fileref=document.createElement("link");
	        fileref.setAttribute("rel", "stylesheet");
	        fileref.setAttribute("type", "text/css");
	        fileref.setAttribute("href", filename);
	    }
	    if (typeof fileref!="undefined"){
	        document.getElementsByTagName("head")[0].appendChild(fileref);
	    }
	}

  	_bindEvents(){
  		
    	this.on(riot.EVT.dynamicJsCssLoaderStore.in.loadExternalJsCss,	this._safeLoadExternal);
    	this.on(riot.EVT.dynamicJsCssLoaderStore.in.unloadExternalJsCss,this._removeExternal);
    	
    }
  
}
export default DynamicJsCssLoaderStore;