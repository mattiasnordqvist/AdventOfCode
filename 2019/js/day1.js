var fs = require('fs');
var path = require('path');
var filePath = path.join(__dirname, 'day1.txt');
var contents = fs.readFileSync(filePath, 'utf8');
var lines = contents.split('\n');

var fuelForMass = mass => (mass/3|0)-2;

// part 1
var sum = lines
    .map(line => fuelForMass(parseInt(line)))
    .reduce((p,f) => p+f, 0);
console.log(sum);

var fuelForMassRecursively = mass =>
{
    var massOfAddFuel = fuelForMass(mass);
    return massOfAddFuel <= 0 ? 0 : massOfAddFuel + fuelForMassRecursively(massOfAddFuel);
}
// part 2
sum = lines
    .map(line => fuelForMassRecursively(parseInt(line)))
    .reduce((p,f) => p+f, 0);
console.log(sum);

