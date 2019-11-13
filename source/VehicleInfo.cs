using System;
using System.Collections.Generic;

[Serializable]
public class Vehicle {
	public string ModelYear;
	public string Model;
	public string Make;
}
[Serializable]
public class VehicleInfo {
	public List<Vehicle> Results;
}