
import RiotControl from 'riotcontrol';
import './components/header.tag';
 
<app>
<header></header>
<div class="container">
	<div id="riot-app"></div>
</div>


<script>
 	var self = this;

  self.on('before-mount', function() {
    // before the tag is mounted
    console.log('app before-mount') // Succeeds, fires once (per mount)
  })

 	self.on('mount', () => {
    console.log('app mount');
   // RiotControl.on(riot.EVT.finalMount,self.onFinalMount);
  });

  self.on('unmount', () => {
    console.log('app unmount')
    //RiotControl.off(riot.EVT.finalMount,self.onFinalMount);
  });
 
</script>
</app>