import '../components/itemlist.tag';

<home> 

<div class="progress">
  <div class="progress-bar progress-bar-info" style="width: {times[0]}%"></div>
</div

<div class="progress">
  <div class="progress-bar progress-bar-success" style="width: {times[1]}%"></div>
</div>

<div class="progress">
  <div class="progress-bar progress-bar-warning" style="width: {times[2]}%"></div>
</div>

<div class="progress">
  <div class="progress-bar progress-bar-danger" style="width: {times[3]}%"></div>
</div>
<div class="progress progress-striped">
  <div class="progress-bar progress-bar-info" style="width: {times[4]}%"></div>
</div>

<div class="progress progress-striped">
  <div class="progress-bar progress-bar-success" style="width: {times[5]}%"></div>
</div>

<div class="progress progress-striped">
  <div class="progress-bar progress-bar-warning" style="width: {times[6]}%"></div>
</div>

<div class="progress progress-striped">
  <div class="progress-bar progress-bar-danger" style="width: {times[7]}%"></div>
</div>
<div class="progress progress-striped active">
  <div class="progress-bar" style="width: {times[8]}%"></div>
</div>

<div>
	<a href="#" class="btn {buttonClasses[buttonS[0]]}">Default</a>
	<a href="#" class="btn {buttonClasses[buttonS[1]]}">Primary</a>
	<a href="#" class="btn {buttonClasses[buttonS[2]]}">Success</a>
	<a href="#" class="btn {buttonClasses[buttonS[3]]}">Info</a>
	<a href="#" class="btn {buttonClasses[buttonS[4]]}">Warning</a>
	<a href="#" class="btn {buttonClasses[buttonS[5]]}">Danger</a>
	<a href="#" class="btn {buttonClasses[buttonS[6]]}">Link</a>
</div>
<div class="spacer"></div>
<div>
	<a class="btn btn-default" onclick={this.generateAnError} >Generate An Error</a>

</div>

<script>
	var self = this;
	self.name = 'home';

	self.times = [0,1,2,3,4,5,6,7,8,9];
	self.buttonS = [0,1,2,3,4,5,6];
	self.buttonClasses = [
		"btn-default",
		"btn-primary",
		"btn-success",
		"btn-info",
		"btn-warning",
		"btn-danger",
		"btn-link"
		];

	self.a = 0;
	self.b = 0;
	self.c = 0;

	self.tick = () => {

	
		var arrayLength = self.times.length;
		for (var i = 0; i < arrayLength; i++) {
		    self.times[i] = Math.floor(Math.random() * (100 - 0) + 0);
		}

		arrayLength = self.buttonS.length;
		for (var i = 0; i < arrayLength; i++) {
		    self.buttonS[i] = Math.floor(Math.random() * (6 - 0) + 0);
		}

		self.a = self.times[0];
		self.b = Math.random() * (100 - self.a) + self.a;
		self.c = 100 - self.a - self.b;
	
  		self.update();
  	};
	
	
	self.on('mount', function() {
		self.tick();
    	self.timer =  setInterval(this.tick,400)
    })
	self.on('unmount', function() {
    	clearInterval(self.timer)
    })

	self.generateAnError = () => {
  		riot.control.trigger('ErrorStore:error-catch-all',{code:'dancingLights-143523'});
  	};
</script>

</home>