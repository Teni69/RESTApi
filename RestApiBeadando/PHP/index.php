<?php 
$method = $_SERVER['REQUEST_METHOD'];

if(array_key_exists('A', $_GET) && $_GET['A'] == 'E76IKD|admin') {

	switch($method) {
		case "GET":
		if(!empty($_GET['id'])) {
			$id = intval($_GET['id']);
			getGames($id);
		} else getGames();
		break;

		case "POST":
		insertGame();
		break;

		case "PUT":
		$id = intval($_GET['id']);
		updateGame($id);
		break;

		case "DELETE":
		$id = intval($_GET['id']);
		deleteGame($id);
		break;

		default:
		header("HTTP/1.0 405 Method Not Allowed");	
		break;
	}

	
} else {
	header('Content-Type: application/json');
	echo json_encode("WRONG AUTH INFO", JSON_UNESCAPED_UNICODE);
}

function getGames($id = 0) {
	$query = "SELECT * FROM games";
	if($id != 0) $query.=" WHERE id=".$id." LIMIT 1";
	include_once 'connection.php';
	$response = getList($query);

	header('Content-Type: application/json');
	echo json_encode($response);
}

function insertGame () {
	$data = json_decode(file_get_contents('php://input'), true);
	$query = "INSERT INTO games (name, genre, price, releaseYear) VALUES (:name, :genre, :price, :releaseYear)";
	$params = [
		":name" => $data["name"],
		":genre" => $data["genre"],
		":price" => $data["price"],
		":releaseYear" => $data["releaseYear"]
	];
	include_once 'connection.php';
	$response = executeDML($query, $params);
	header('Content-Type: application/json');
	echo json_encode($response, JSON_UNESCAPED_UNICODE);
}

function updateGame($id) {
	$data = json_decode(file_get_contents('php://input'), true);
	$query = "UPDATE games SET name = :name, genre = :genre, price = :price, releaseYear = :releaseYear WHERE id = :id";
	$params = [
		":name" => $data["name"],
		":genre" => $data["genre"],
		":price" => $data["price"],
		":releaseYear" => $data["releaseYear"],
		":id" => $id
	];
	include_once 'connection.php';
	$response = executeDML($query, $params);
	header('Content-Type: application/json');
	echo json_encode($response, JSON_UNESCAPED_UNICODE);
}

function deleteGame($id) {
	$query = "DELETE FROM games WHERE id=".$id;
	include_once 'connection.php';
	$response = getList($query);

	header('Content-Type: application/json');
	echo json_encode($response, JSON_UNESCAPED_UNICODE);
}	
?>