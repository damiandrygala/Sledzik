export function findCenterPointOnMap(coordinates) {
  if (coordinates.length != 0) {
  var minLatitude = coordinates[0][0].latitude,
    maxLatitude = 0,
    minLongitude = coordinates[0][0].longitude,
    maxLongitude = 0;
  coordinates.forEach((coords) => {
    coords.forEach((coordinate) => {
      if (coordinate.latitude < minLatitude) minLatitude = coordinate.latitude;
      if (coordinate.latitude > maxLatitude) maxLatitude = coordinate.latitude;
      if (coordinate.longitude < minLongitude)
        minLongitude = coordinate.longitude;
      if (coordinate.longitude > maxLongitude)
        maxLongitude = coordinate.longitude;
    });
  });
  return [(maxLatitude + minLatitude) / 2, (minLongitude + maxLongitude) / 2];
  }
  else return [51.0783027, 17.0619564]
}
