import * as nprogress from 'nprogress';
import 'nprogress/nprogress.css'

<loading-indicator>

<script>
    var self = this;
    self.onProgressStart = () =>{
        nprogress.start();
    }
    self.onProgressDone = () =>{
        nprogress.done();
    }
    self.on('mount', function() {
        console.log('loading-indicator mount......')
        riot.control.on(riot.EVT.progressStore.out.progressStart, self.onProgressStart);
        riot.control.on(riot.EVT.progressStore.out.progressDone, self.onProgressDone);
    });

    self.on('unmount', function() {
        console.log('loading-indicator unmount......')
        riot.control.off(riot.EVT.progressStore.out.progressStart, self.onProgressStart);
        riot.control.off(riot.EVT.progressStore.out.progressDone, self.onProgressDone);
    });

</script>
</loading-indicator>