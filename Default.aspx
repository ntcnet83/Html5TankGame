<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Html5TankGame.Default" %>
<!DOCTYPE html>
<html>
<head>
    <title>Tank Game</title>
    <script type="text/javascript" src="Scripts/jquery-1.8.2.min.js"></script>
    <script type="text/javascript" src="Scripts/json2.min.js"></script>
    <script type="text/javascript" src="Scripts/jquery.signalR-0.5.3.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            var TANK_BODY_SCALE = <%= Html5TankGame.Game.TANK_BODY_SCALE.ToString() %>;
            var TANK_TURRET_SCALE = <%= Html5TankGame.Game.TANK_TURRET_SCALE.ToString() %>;
            var BULLET_SIZE = <%= Html5TankGame.Game.BULLET_SIZE %>;
            var TANK_HULL_SCALE = <%= Html5TankGame.Game.TANK_HULL_SCALE %>;
            var debug = $('#debug');
            var divScores = $('#scores');
            var arena;
            var scores = [];
            var missles = [];
            var tanks = [];
            var gameConnection = $.connection('/game');
            gameConnection.start();
            gameConnection.received(function (data) {
                tanks = data.Tanks;
                arena = data.Arena;
                missles = data.Missles;
                scores = data.Scores;
                debug.html(data.Debug);
                refreshScreen();
            });

            function refreshScreen() {
                var boundary = arena.Boundary;
                game_area.width = boundary.Width;
                game_area.height = boundary.Height;
                context = game_area.getContext('2d');
                context.clearRect(boundary.X, boundary.Y, boundary.Width, boundary.Height);
                drawArena();
                drawTanks();
                drawMissles();
                drawScores();
            }

            function drawScores(){
                var s = '';
                for (var i = 0; i < scores.length; i++) {
                    s += scores[i].ControllerName + ' - ' + scores[i].Wins + '<br/>';
                }
                divScores.html(s);
            }

            function drawArena() {
                context.save();
                context.beginPath();
                context.strokeStyle = "yellow";
                
                // Draw the frame
                var border = arena.Border;
                context.strokeRect(border.X, border.Y, border.Width, border.Height);
                context.closePath();
                context.stroke();
                context.restore();
            }

            function drawTanks() {
                for (var i = 0; i < tanks.length; i++)
                    drawTank(tanks[i]);
            }

            function drawTank(tank) {
                context.save();

                context.beginPath();
                context.arc(tank.X, tank.Y, (tank.Size * TANK_HULL_SCALE), 0, 2 * Math.PI, false);
                context.fillStyle = "rgb(" + tank.R + "," + tank.G + "," + tank.B + ")";
                context.fill();
                context.lineWidth = 1;
                context.strokeStyle = "rgb(" + tank.R + "," + tank.G + "," + tank.B + ")";
                context.stroke();
                //box
                context.moveTo(
                    tank.X + (Math.floor(Math.sin(tank.Angle + (Math.PI * .25)) * tank.Size * TANK_BODY_SCALE)), 
                    tank.Y + (Math.floor(Math.cos(tank.Angle + (Math.PI * .25)) * tank.Size * TANK_BODY_SCALE)));
                context.lineTo(
                    tank.X + (Math.floor(Math.sin(tank.Angle + (Math.PI * .75)) * tank.Size * TANK_BODY_SCALE)), 
                    tank.Y + (Math.floor(Math.cos(tank.Angle + (Math.PI * .75)) * tank.Size * TANK_BODY_SCALE)));
                context.lineTo(
                    tank.X + (Math.floor(Math.sin(tank.Angle + (Math.PI + (Math.PI * .25))) * tank.Size * TANK_BODY_SCALE)), 
                    tank.Y + (Math.floor(Math.cos(tank.Angle + (Math.PI + (Math.PI * .25))) * tank.Size * TANK_BODY_SCALE)));
                context.lineTo(
                    tank.X + (Math.floor(Math.sin(tank.Angle + (Math.PI + (Math.PI * .75))) * tank.Size * TANK_BODY_SCALE)), 
                    tank.Y + (Math.floor(Math.cos(tank.Angle + (Math.PI + (Math.PI * .75))) * tank.Size * TANK_BODY_SCALE)));
                context.lineTo(
                    tank.X + (Math.floor(Math.sin(tank.Angle + (Math.PI * .25)) * tank.Size * TANK_BODY_SCALE)), 
                    tank.Y + (Math.floor(Math.cos(tank.Angle + (Math.PI * .25)) * tank.Size * TANK_BODY_SCALE)));
                context.stroke();
                //turret
                context.moveTo(tank.X, tank.Y);
                context.lineTo(tank.X + Math.floor(Math.sin(tank.TurretAngle) * (tank.Size * TANK_TURRET_SCALE)), tank.Y + Math.floor(Math.cos(tank.TurretAngle) * (tank.Size * TANK_TURRET_SCALE)));
                context.stroke();
                
                context.restore();
            }

            function drawMissles() {
                context.save();
                for (var i = 0; i < missles.length; i++) {
                    drawMissle(missles[i]);
                }
                context.restore();
            }

            function drawMissle(missle) {
                context.beginPath();
                context.fillStyle = "rgb(" + missle.R + "," + missle.G + "," + missle.B + ")";
                context.fillRect(missle.X, missle.Y, BULLET_SIZE, BULLET_SIZE);
                context.stroke();
            }
        });
    </script>
    <style type="text/css">
        #game_area
        {
            background-color: #000000;
        }
    </style>
</head>
<body>
    <div id="scores"></div>
    <canvas id="game_area"></canvas>
    <div id="debug" ></div>
</body>
</html>
