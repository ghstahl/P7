<startup>
<script>
	var self = this;
  if(self.opts.config){
    self.config = self.opts.config;
  }
  self.loaded = false;
  self.on('mount', () => {
    riot.control.on('startup-tag-fetch-config-ack',
                    self.onStartupTagFetchConfigAck);
    riot.control.trigger('StartupStore:fetch-config',self.config,
      {evt:'startup-tag-fetch-config-ack'});
  });

  self.on('unmount', () => {
    riot.control.off('startup-tag-fetch-config-ack',
                    self.onStartupTagFetchConfigAck);
  });
  self.onStartupTagFetchConfigAck = () =>{
    if(!self.loaded){
      self.loaded = true;
      riot.control.off('startup-tag-fetch-config-ack',
                    self.onStartupTagFetchConfigAck);
      riot.control.trigger(riot.EVT.startupStore.in.start);
    }
  }
    
</script>
</startup>