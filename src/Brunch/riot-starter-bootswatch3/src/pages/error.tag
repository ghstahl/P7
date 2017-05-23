<error>
  <div class="panel panel-default">
    <div class="panel-heading">
      <h3 class="panel-title">Error Code: {state.code} </h3>
    </div>
    <div class="panel-body">
      <div class="alert alert-danger">
          <strong>Well Hell!</strong> We have dispatched the minions to determine who was responsible for this defect.  Once they have been dealt with, we will fix the issue.
        </div>
    </div>
  </div>

<script>
	var self = this;
  self.name = "error";
	self.error = false;

  self.on('before-mount', () => {
    self.state = riot.state.error;
  });

	self.on('mount', () => {
    console.log(self.name,'mount') 
  });

  self.on('unmount', () => {
    console.log(self.name,'unmount')
  });

</script>

</error>