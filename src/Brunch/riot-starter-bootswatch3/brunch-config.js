exports.paths = {
     public: './public/app'
}

// See http://brunch.io for documentation.
exports.files = {
    javascripts: {
        joinTo: {
            'app.js': /^app/,
            'vendor.js': /^(?!app)/ // all BUT app code - 'vendor/', 'node_modules/', etc
        }
    },
    stylesheets: {joinTo: 'app.css'}
};

exports.plugins = {
    babel: {presets: ['latest', 'stage-0']},
    uglify: {
      mangle: false,
      compress: {
        global_defs: {
          DEBUG: false
        }
      }
    }
};

exports.npm = {
    enabled: true,
    globals: {
        jQuery: 'jquery',
        $: 'jquery',
        Tether:'tether',
        bootstrap: 'bootstrap',
        riot: 'riot' 

    },
    styles: {
        bootswatch: ['slate/bootstrap.css']
    }
};
