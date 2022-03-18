import javax.swing.ImageIcon;
import javax.swing.JFrame;
import javax.imageio.ImageIO;
import java.io.File;
import java.io.IOException;

import javax.swing.JComponent;
import java.awt.Image;
import java.awt.Graphics;
import java.awt.image.BufferedImage;

public class App extends JFrame{

    private ImageIcon icono = new ImageIcon(System.getProperty("user.dir") + "/icono.png");
    private final int HEIGHT = 500;
    private final int WIDTH = 500;

    public App(){
        setTitle("TP1");
        setIconImage(icono.getImage());
        setSize(HEIGHT, WIDTH); //estaria bueno poder borrar estos margenes, pero eso es un problema para el futuro
        setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        setResizable(false);
        setLocationRelativeTo(null);

        try{
            BufferedImage myImage = ImageIO.read(new File(System.getProperty("user.dir") + "/stonks.jpg"));
            setContentPane(new ImagePanel(scale(myImage, HEIGHT, WIDTH)));
            
        } catch(IOException e){
            e.printStackTrace();
        }
        setVisible(true);
    }

    private BufferedImage scale(BufferedImage src, int w, int h){
    BufferedImage img = 
            new BufferedImage(w, h, BufferedImage.TYPE_INT_RGB);
    int x, y;
    int ww = src.getWidth();
    int hh = src.getHeight();
    int[] ys = new int[h];
    for (y = 0; y < h; y++)
        ys[y] = y * hh / h;
    for (x = 0; x < w; x++) {
        int newX = x * ww / w;
        for (y = 0; y < h; y++) {
            int col = src.getRGB(newX, ys[y]);
            img.setRGB(x, y, col);
        }
    }
    return img;
}

    class ImagePanel extends JComponent {
        private Image image;
        public ImagePanel(Image image) {
            this.image = image;
        }
        @Override
        protected void paintComponent(Graphics g) {
            super.paintComponent(g);
            g.drawImage(image, 0, 0, this);
        }
    }
}

/*
import java.io.*;
import java.net.*;

public class c {

   public static String getHTML() throws Exception {
     String moneda; //par de referencia
      StringBuilder result = new StringBuilder();
      URL url = new URL("https://api.binance.com/api/v3/avgPrice?symbol=BTCUSDT");
      HttpURLConnection conn = (HttpURLConnection) url.openConnection();
      conn.setRequestMethod("GET");
      try (BufferedReader reader = new BufferedReader(
                  new InputStreamReader(conn.getInputStream()))) {
          for (String line; (line = reader.readLine()) != null; ) {
              result.append(line);
          }
      }
      return result.toString();
   }

   public static void main(String[] args) throws Exception
   {
     System.out.println(getHTML());
   }
}
*/