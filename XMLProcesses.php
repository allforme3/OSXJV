<?
ini_set('upload_max_filesize', '200M');
ini_set('post_max_size', '200M');
ini_set('max_input_time', 900);
ini_set('max_execution_time', 900);

$target_dir = "uploads/";
$target_file = $target_dir . basename($_FILES['file']['name']);
$uploadOk = 1;
$imageFileType = pathinfo($target_file,PATHINFO_EXTENSION); // get file type
switch($imageFileType)
{
    //Restrict File Type
    case "xml";
    case "json":
    case "xsd":
        break;
    default:
        $uploadOk = 0;
        die(json_encode([ 'success'=> $uploadOk, 'error'=> "Wrong File Type"]));
}

if (move_uploaded_file($_FILES["file"]["tmp_name"], $target_file)) {
    die(json_encode([ 'success'=> $uploadOk, 'error'=> "", 'data' => "File Uploaded Succesfully" ]));
}
else
{
    $uploadOk = 0;
    $error_msg = "Error With File";
}
die(json_encode([ 'success'=> $uploadOk, 'error'=> $error_msg]));
?>

