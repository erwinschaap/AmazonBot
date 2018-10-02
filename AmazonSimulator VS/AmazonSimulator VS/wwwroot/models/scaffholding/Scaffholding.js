class Scaffholding extends THREE.Group {
    constructor() {
        super();
        this.init();
    }

    init() {
        var selfRef = this;

        var mtlLoader = new THREE.MTLLoader();
        mtlLoader.setPath('models/scaffholding/');
        var url = "Present_house.mtl";
        mtlLoader.load(url, function (materials) {

            materials.preload();
            var objLoader = new THREE.OBJLoader();
            objLoader.setMaterials(materials);
            objLoader.setPath('models/scaffholding/');
            objLoader.load('Present_house.obj', function (object) {
                var group = new THREE.Group();
                object.position.y = 1.5;
                group.add(object);
                selfRef.add(group);
            });

        });
    }
}