<itemlist>
  <div class="row">
    <div class="col-md-6">
      <h3>{ opts.title }</h3>

      <ul>
        <li each={ items } >{ this.name }</li>
      </ul>
    </div>

  </div>

  <script>
    var self = this;
    self.items = [];

    self.onLoadItemsSuccess = (items) =>{
      self.items = items;
      self.update()
    }

    self.on('unmount', () => {
       console.log('itemList unmount');
       riot.control.off(riot.EVT.loadItemsSuccess, self.onLoadItemsSuccess);
    });

   
    self.on('mount', () => {
      console.log('itemList mount');
      riot.control.on(riot.EVT.loadItemsSuccess, self.onLoadItemsSuccess);
      riot.control.trigger(riot.EVT.loadItems);
    });

    
  </script>
</itemlist>