var mtlLoader = new THREE.MTLLoader();
mtlLoader.setTexturePath('\models');
mtlLoader.setPath('\models');
mtlLoader.load('Santasleigh.mtl', function (materials) {

    materials.preload();

    var objLoader = new THREE.OBJLoader();
    objLoader.setMaterials(materials);
    objLoader.setPath('\models');
    objLoader.load('Santasleigh.obj', function (object) {

        scene.add(object);
        object.position.y -= 60;

    });

});