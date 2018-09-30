class Truck extends THREE.Group {
    constructor() {
        super();
        this.init();
    }

    init() {
        var selfRef = this;

        var mtlLoader = new THREE.MTLLoader();
        mtlLoader.setPath('models/truck/');
        var url = "Santasleigh.mtl";
        mtlLoader.load(url, function (materials) {

            materials.preload();
            var objLoader = new THREE.OBJLoader();
            objLoader.setMaterials(materials);
            objLoader.setPath('models/truck/');
            objLoader.load('Santasleigh.obj', function (object) {
                var group = new THREE.Group();
                object.position.y = 0.4;
                group.add(object);
                selfRef.add(group);
            });

        });
    }

}
