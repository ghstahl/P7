class RiotRouteExtension{
  constructor(){
    var self = this;
    self.name = 'RiotRouteExtension';
    self.namespace = self.name+':';
    self.currentPath = '';

    self._defaultParser = (path) =>{
      self.currentPath = path;
      return path.split(/[/?#]/)
    }
    self._getCurrentPath = () => {
      return self.currentPath;
    }
    riot.route.parser(self._defaultParser,null);
    riot.route.currentPath = self._getCurrentPath;
  }
}
export default RiotRouteExtension;