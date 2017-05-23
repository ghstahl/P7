/**
 * Created by Herb on 9/27/2016.
 */


function ProgressStore() {
    var self = this
    self.name = 'ProgressStore';
    self.namespace = self.name + ':';

    riot.observable(self) // Riot provides our event emitter.

    riot.EVT.progressStore ={
        in:{
            inprogressDone:self.namespace+'inprogress-done',
            inprogressStart:self.namespace+'inprogress-start'
        },
        out:{
            progressStart:self.namespace+'progress-start',
            progressCount:self.namespace+'progress-count',
            progressDone:self.namespace+'progress-done'
        }
        
    }

    self.count = 0;

    self.on(riot.EVT.progressStore.in.inprogressStart, function() {
        if(self.count == 0){
            self.trigger(riot.EVT.progressStore.out.progressStart)
        }
        ++self.count;
        self.trigger(riot.EVT.progressStore.out.progressCount,self.count);
    })

    self.on(riot.EVT.progressStore.in.inprogressDone, function() {
        if(self.count == 0){
            // very bad.
            console.error(riot.EVT.progressStore.in.inprogressDone,'someone has their inprogress_done mismatched with thier inprogress_start');
        }
        if(self.count > 0){
            --self.count;
        }
        self.trigger(riot.EVT.progressStore.out.progressCount,self.count);
        if(self.count == 0){
            self.trigger(riot.EVT.progressStore.out.progressDone)
        }
    })
}


if (typeof(module) !== 'undefined') module.exports = ProgressStore;



